import socket
import random
import keyboard

HOST = '127.0.0.1'
PORT = 50007

client = socket.socket( socket.AF_INET, socket.SOCK_DGRAM )
client.bind( ( HOST, PORT ) )
client.listen()

connection, adress = client.accept()

while (True):
    if( keyboard.is_pressed( 'q' ) ):
        break

    recieve = connection.recv( 4096 ).decode()
    if( recieve != None ):
        result = 'ノーテン'
        connection.sendto( result.encode('utf-8') )