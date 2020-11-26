import pyaudio
import io
import wave
import scipy.io.wavfile as wav
from array import array
import time
from deepspeech import Model
import sys
import time

current_milli_time = lambda: int(round(time.time() * 1000))

FORMAT=pyaudio.paInt16
CHANNELS=2
RATE=16000
CHUNK=1024
RECORD_SECONDS=15
FILE_NAME="RECORDING.wav"


lastDetection = 0
startRecording = 0
endRecording = 0
newAudioDetected = False

audio=pyaudio.PyAudio() #instantiate the pyaudio

#recording prerequisites
stream=audio.open(format=FORMAT,channels=CHANNELS, 
                  rate=RATE,
                  input=True,
                  frames_per_buffer=CHUNK)

#Carrega o modelo
ds = Model(sys.argv[1], 500)

#Inicia a gravacao
frames=[]

#inicia a deteccao
newAudioDetected = False
lastDetection = current_milli_time()

while True:
    #Captura o audio
    data=stream.read(CHUNK)
    data_chunk=array('h',data)
    vol = max(data_chunk)
    #Deteccao de volume:
    if(vol >= 800):
	lastDetection = current_milli_time()
	newAudioDetected = True

	if(startRecording == 0):
		startRecording = current_milli_time()	
	print(".") #Comecou!

    if(((current_milli_time()-lastDetection) < 2000) and newAudioDetected):
	frames.append(data)
	#print("...")	
    else:
	if(newAudioDetected):
		if((current_milli_time() - startRecording) > 2000):
			print("saved")
			newAudioDetected = False
			
			#Salva o Buffer e processa a informacao
			temp_file = io.BytesIO()
			wavfile = wave.open(temp_file,'wb')
			wavfile.setnchannels(CHANNELS)
			wavfile.setsampwidth(audio.get_sample_size(FORMAT))
			wavfile.setframerate(RATE)
			wavfile.writeframes(b''.join(frames))
			temp_file.seek(0)
			fs, waveVector = wav.read(temp_file)
			processed_data = ds.stt(waveVector[:,0])
			print(processed_data)
			#'''
			     #Finaliza a operacao
		frames=[]
		newAudioDetected = False		
		startRecording = 0;
	#print("Nada a Fazer")


#end of recording
stream.stop_stream()
stream.close()
audio.terminate()