#!/usr/bin/env python
import paho.mqtt.client as mqtt
import matplotlib.pyplot as plt


topic_pos = "position_data"
topic_sensor_data = "sensor_data"

img = plt.imread("background.jpeg")
ext = [0.0, 7468.0, 0.00, 9650.0]
plt.imshow(img, zorder=0, extent=ext)
aspect=img.shape[0]/float(img.shape[1])*((ext[1]-ext[0])/(ext[3]-ext[2]))
plt.gca().set_aspect(aspect)
plt.show()

x_vals = []
y_vals = []

def animate(x,y):
    x_vals.append(x)
    y_vals.append(y)
    plt.cla()
    plt.plot(x_vals, y_vals)

def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))
    client.subscribe(topic_pos)
    client.subscribe(topic_sensor_data)

def on_message(client, userdata, msg):

    msg_new = str(msg.payload)
    msg_new = msg_new.replace("b'[", "").replace("]'", "").split(", ")
    x = msg_new[0]
    y = msg_new[1]
    z = msg_new[2]
    animate(x,y)

    if(x != "0" and y != "0" and z != "0"):
        my_file = open('C:\HTL\Huss\Robotino_Pozyx-master\mqtt_subscriber\Test.txt', 'w')
        my_file.write(x+" "+y+" "+z)

client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("172.17.241.103", 1883, 60)

client.loop_forever()
