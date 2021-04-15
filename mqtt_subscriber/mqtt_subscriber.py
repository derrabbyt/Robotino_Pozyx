#!/usr/bin/env python
import paho.mqtt.client as mqtt


topic_pos = "position_data"
topic_sensor_data = "sensor_data"
f = open("Test.txt", "w")


def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))
    client.subscribe(topic_pos)
    #client.subscribe(topic_sensor_data)

def on_message(client, userdata, msg):
    f.write(str(msg.payload))
    print(str(msg.payload))


client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect("192.168.1.1", 1883, 60)

client.loop_forever()