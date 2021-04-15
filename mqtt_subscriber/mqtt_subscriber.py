#!/usr/bin/env python
import paho.mqtt.client as mqtt

topic_pos = "position_data"
topic_sensor_data = "sensor_data"

def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))

    client.subscribe(topic_pos)
    #client.subscribe(topic_sensor_data)

def on_message(client, userdata, msg):

    msg_new = str(msg.payload)
    msg_new = msg_new.replace("b'[", "")
    msg_new = msg_new.replace("]'", "")
    msg_new = msg_new.split(", ")
    x = msg_new[0]
    y = msg_new[1]
    z = msg_new[2]
    if(x != "0" and y != "0" and z != "0"):
        my_file = open('C:\HTL\Huss\Robotino_Pozyx-master\mqtt_subscriber\Test.txt', 'w')
        print(x)
        print(y)
        print(z)
        print("")
        my_file.write(x+" "+y+" "+z)



    #print(msg.topic + " " + str(msg.payload))

client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("172.17.241.103", 1883, 60)

client.loop_forever()
