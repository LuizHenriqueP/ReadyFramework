import sys
import time
import rospy
from array import array
from std_msgs.msg import String
import os


voicePublisher = rospy.Publisher('voiceOutput', String, queue_size=10)


def publishMessage(message):
    voicePublisher.publish(message)



def listenNode(data):
    print("callback " + data.data)
    if  data.data: 
        print("message receiveid")
        os.system("espeak " + '"' + data.data + '"')
        publishMessage("")


def listener():
    rospy.init_node('listener', anonymous=True)
    voiceSubscriber  = rospy.Subscriber('voiceOutput', String, listenNode)
    while not rospy.is_shutdown():
        print("listening")
        rospy.spin()



if __name__ == '__main__':
    listener()


