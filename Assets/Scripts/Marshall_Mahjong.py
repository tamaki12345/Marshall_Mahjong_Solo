import socket

my_address = '127.0.0.1'
my_port = 50000
unity_port = 9000

socket = socket.socket( socket.AF_INET, socket.SOCK_DGRAM )
socket.bind( ( my_address, my_port ) )

while(True):
    try:
        print(f'waiting connect \nport:{my_port} \naddress:{my_address}')
        recieve, (senderIP, senderPORT) = socket.recvfrom( 4096 )
        recieve = recieve.decode()
        print('recieved\n',recieve)

        if( recieve != None ):
            result = 'ノーテン'
            socket.sendto( result.encode('utf-8'), (my_address, unity_port) )
            break
        
    except KeyboardInterrupt:
        print('\n...\n')
        socket.close()
        break