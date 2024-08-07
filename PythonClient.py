import socket

host, port = "127.0.0.1", 25001
data = "1,2,3"
source_file_path = "C:/Users/Mihai/Downloads/CharacterLowPoly.unitypackage"

data_to_send = f"{data},{source_file_path}"

# TCP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

try:
    # Connect
    sock.connect((host, port))
    sock.sendall(data_to_send.encode("utf-8"))
    response = sock.recv(1024).decode("utf-8")
    print(response)

finally:
    sock.close()
