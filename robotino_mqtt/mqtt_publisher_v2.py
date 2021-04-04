import paho.mqtt.client as mqtt
from time import sleep
from pypozyx import (POZYX_POS_ALG_UWB_ONLY, POZYX_2_5D, Coordinates, POZYX_SUCCESS, PozyxConstants, version,
                     DeviceCoordinates, PozyxSerial, get_first_pozyx_serial_port, SingleRegister, DeviceList,
                     PozyxRegisters, EulerAngles)
from pypozyx import SensorData, SingleRegister, POZYX_SUCCESS, get_first_pozyx_serial_port, PozyxSerial, \
    get_serial_ports, Acceleration
from pypozyx.definitions.bitmasks import POZYX_INT_MASK_IMU
from pythonosc.udp_client import SimpleUDPClient
from pypozyx.tools.version_check import perform_latest_version_check
topic_postion_data = "position_data"
topic_sensor_data = "sensor_data"
client = mqtt.Client()

class ReadyToLocalize(object):
    """Continuously calls the Pozyx positioning function and prints its position."""

    def __init__(self, pozyx, osc_udp_client, anchors, algorithm=POZYX_POS_ALG_UWB_ONLY, dimension=POZYX_2_5D, height=1000, remote_id=None):
        self.pozyx = pozyx
        self.osc_udp_client = osc_udp_client
        self.anchors = anchors
        self.algorithm = algorithm
        self.dimension = dimension
        self.height = height
        self.remote_id = remote_id

    def on_connect(client, userdata, flags, rc):
        print("Connected with result code " + str(rc))

    client.on_connect = on_connect
    client.connect("localhost", 1883, 60)
    client.loop_start()

    def setup(self):
        self.setAnchorsManual(save_to_flash=False)
        self.printPublishConfigurationResult()

    def loop(self):
        position = Coordinates()
        sensor_data = SensorData()

        status = self.pozyx.doPositioning(
            position, self.dimension, self.height, self.algorithm, remote_id=self.remote_id)

        if status == POZYX_SUCCESS:
            self.publishData(position, sensor_data)

        calibration_status = SingleRegister()
        if self.remote_id is not None or self.pozyx.checkForFlag(POZYX_INT_MASK_IMU, 0.01) == POZYX_SUCCESS:
            status = self.pozyx.getAllSensorData(sensor_data, self.remote_id)
            status &= self.pozyx.getCalibrationStatus(calibration_status, self.remote_id)

        self.publishData(position, sensor_data)  # nur ohne anchors

    #anchor setup
    def setAnchorsManual(self, save_to_flash=False):
        """Adds the manually measured anchors to the Pozyx's device list one for one."""
        status = self.pozyx.clearDevices(remote_id=self.remote_id)
        for anchor in self.anchors:
            status &= self.pozyx.addDevice(anchor, remote_id=self.remote_id)
        if len(self.anchors) > 3:
            status &= self.pozyx.setSelectionOfAnchors(PozyxConstants.ANCHOR_SELECT_AUTO, len(self.anchors),
                                                       remote_id=self.remote_id)
        if save_to_flash:
            self.pozyx.saveAnchorIds(remote_id=self.remote_id)
            self.pozyx.saveRegisters([PozyxRegisters.POSITIONING_NUMBER_OF_ANCHORS], remote_id=self.remote_id)
        return status

    #übergibt die position (und senosr data) an den subscriber (laptop)
    def publishData(self, position, sensor_data):
        """Prints the Pozyx's position and possibly sends it as a OSC packet"""
        network_id = self.remote_id
        if network_id is None:
            network_id = 0

        print("should send")
        client.publish(topic_postion_data, "POS ID {}, x(mm): {pos.x} y(mm): {pos.y} z(mm): {pos.z}".format("0x%0.4x" % network_id,
                                                                                               pos=position))
        #client.publish(topic_sensor_data, sensor_data.linear_acceleration.x)

if __name__ == "__main__":
    # Check for the latest PyPozyx version. Skip if this takes too long or is not needed by setting to False.
    check_pypozyx_version = True
    if check_pypozyx_version:
        perform_latest_version_check()

    # shortcut to not have to find out the port yourself
    serial_port = get_first_pozyx_serial_port()
    if serial_port is None:
        print("No Pozyx connected. Check your USB cable or your driver!")
        quit()

    remote_id = 0x6751                 # remote device network ID
    remote = False                   # whether to use a remote device
    if not remote:
        remote_id = None

    # enable to send position data through OSC
    use_processing = True

    # configure if you want to route OSC to outside your localhost. Networking knowledge is required.
    ip = "127.0.0.1"
    network_port = 8888

    osc_udp_client = None
    if use_processing:
        osc_udp_client = SimpleUDPClient(ip, network_port)

    # necessary data for calibration, change the IDs and coordinates yourself according to your measurement
    anchors = [DeviceCoordinates(0x670c, 1, Coordinates(5933, 9600, 2200)),
               DeviceCoordinates(0x6711, 1, Coordinates(200, 0, 2989)),
               DeviceCoordinates(0x674b, 1, Coordinates(5409, 0, 2220))]

    # positioning algorithm to use, other is PozyxConstants.POSITIONING_ALGORITHM_TRACKING
    algorithm = PozyxConstants.POSITIONING_ALGORITHM_UWB_ONLY
    # positioning dimension. Others are PozyxConstants.DIMENSION_2D, PozyxConstants.DIMENSION_2_5D
    dimension = PozyxConstants.DIMENSION_2_5D
    # height of device, required in 2.5D positioning
    height = 1000

    pozyx = PozyxSerial(serial_port)
    r = ReadyToLocalize(pozyx, osc_udp_client, anchors, algorithm, dimension, height, remote_id)
    r.setup()
    while True:
        r.loop()