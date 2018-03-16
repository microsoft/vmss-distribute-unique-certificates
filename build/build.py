# cd <root>
# python build/build.py

import base64
import os
import tarfile
from azure.storage.blob import BlockBlobService
from config import Config

config = Config()

vm_files = ["scripts/download_certificate.py",
            "scripts/KeyVaultWrapper.py",
            "scripts/OpenSSLWrapper.py",
            "scripts/config.py"]

# Zip up all python files under the app folder
with tarfile.open("vm_scripts.tar.gz", "w:gz") as tar:
    for f in vm_files:
        tar.add(f)

print 'vm_scripts.tar.gz files created.'

blob_service = BlockBlobService(account_name=config.storage_account_name,
                                sas_token=config.sas_token)

blob_service.create_container(container_name='vm-scripts-files')

blob_service.create_blob_from_path(container_name='vm-scripts-files',
                                   blob_name='vm_scripts.tar.gz',
                                   file_path='vm_scripts.tar.gz')
print 'app.tar.gz is uploaded'

blob_service.create_blob_from_path(container_name='vm-scripts-files',
                                   blob_name='bootstrap.sh',
                                   file_path='scripts/bootstrap.sh')
print 'bootstrap.sh is uploaded'