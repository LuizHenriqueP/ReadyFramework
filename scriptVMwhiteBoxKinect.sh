sudo sh -c 'echo "deb http://packages.ros.org/ros/ubuntu $(lsb_release -sc) main" > /etc/apt/sources.list.d/ros-latest.list'
sudo apt-key adv --keyserver 'hkp://keyserver.ubuntu.com:80' --recv-key C1CF6E31E6BADE8868B172B4F42ED6FBAB17C654
sudo apt-get update
sudo apt-get install ros-kinetic-desktop-full --yes
sudo rosdep init
rosdep update
echo "source /opt/ros/kinetic/setup.bash" >> ~/.bashrc
source ~/.bashrc
sudo apt-get install ros-kinetic-catkin --yes
sudo apt  install python-rosinstall python-rosinstall-generator python-wstool build-essential --yes
sudo apt-get install ros-kinetic-rosbridge-server --yes
sudo apt-get install python-xlib --yes

wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install apt-transport-https --yes
sudo apt-get install dotnet-sdk-2.2 --yes

curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/vscode stable main" > /etc/apt/sources.list.d/vscode.list'
sudo apt update
sudo apt install code --yes 

sudo rosdep fix-permissions
rosdep update

mkdir -p ~/catkin_ws/src
sudo chmod -R 777 ~/catkin_ws/src #pra não dar pau no catkin_make

wget -c https://github.com/siemens/ros-sharp/archive/master.zip
unzip master.zip
mv ros-sharp-master/ROS/* ~/catkin_ws/src

#cria o script na pasta do catkin_wa
echo "catkin_make
echo \"source devel/setup.bash\"
echo \"Se Der pau rode o comando 'sudo chmod -R 777 ~/catkin_ws/src' no diretorio acima\" >> ~/catkin_ws/EXECUTEME.sh

echo "Não esqueça de roda os seguintes comandos:"
echo "Reinicie o pc, se não ele vai dizer que o catkin não está instalado"
echo "vá a pasta catkin_ws e rode o script 'EXECUTEME'"
