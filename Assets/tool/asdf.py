import pandas as pd
import psutil
import time

'''
NOTICE : NEED psutil module
please try 'pip install psutil' in CMD.
'''

#INPUT YOUR CSV FILE PATH.
#You Can try 'C:/filePath/fileName.csv'
filePath = "../Resources/DialogsStorage.csv"

#INPUT YOUR PROCESS(Excel, HanShow... Etc)
# DON'T FORGET .exe
processName = "HCell.exe"

def ANSIToUTF():
    df = pd.read_csv(filePath, encoding='ANSI')
    df.to_csv(filePath, encoding='utf-8', index=False)
    print("done")

def IsProcessRunning(_processName):
    for process in psutil.process_iter(['pid','name']):
        if process.info['name'] == _processName:
            return True
    return False

processOn = False


while True:
    if(processOn):
        if not IsProcessRunning(processName):
            #ON -> OFF
            time.sleep(3)
            try:
                ANSIToUTF()
            except:
                print("maybe, already UTF-8")
            processOn = False
            print("turnOff")
    else:
        if IsProcessRunning(processName):
            processOn = True
            print("turnOn")
    print(processOn)
    time.sleep(1)
