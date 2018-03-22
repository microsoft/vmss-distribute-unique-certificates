yum install -y wget
wget https://bootstrap.pypa.io/get-pip.py
python get-pip.py

pip install azure-keyvault
pip install pyopenssl

tar -xzf vm_scripts.tar.gz
cd scripts
python download_certificate.py &
