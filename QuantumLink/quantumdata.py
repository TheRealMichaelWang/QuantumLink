import socket;
from socket import socket;
from socket import AF_INET;

#connection information.
IP = '127.0.0.1';
PORT = 25565;
CONN = False;
s = None;
BUFFER_SIZE = 1024;
API_NAME = None;

#returns true if the connection was succesful, otherwise it returns false.
def connect():
    global s;
    global CONN;
    try:
        s = socket(AF_INET);
        s.connect((IP,PORT));
        CONN = True;
        return True;
    except:
        return False;

#you must be connected to disconnect
def disconnect():
    global s;
    global CONN;
    if CONN == True:
        run_command("close");
        s.close();

#command runner to server
def run_command(command):
    global s;
    global CONN;
    global BUFFER_SIZE;
    if CONN == True:
        s.send(bytearray(command,'ascii'));
        data = s.recv(BUFFER_SIZE);
        return data.decode('ascii');
    else:
        return "fail";

#returns true if the login completed, false if the login failed.
def login(username,password):
    resp = run_command("login\t"+username+"\t"+password);
    if resp == "fail":
        return False;
    else:
        return True;

#writes data to the database;
def write(key,data):
    resp = run_command("writedata\t"+API_NAME+"\t"+key+"\t"+data);
    if resp == "fail":
        return False;
    else:
        return True;

#reads data from the database;
def read(key):
    resp = run_command("readdata\t"+API_NAME+"\t"+key);
    if resp == "fail":
        return False;
    else:
        return resp;
