from config import Config
from KeyVaultWrapper import KeyVaultWrapper
from OpenSSLWrapper import OpenSSLWrapper

import json
import argparse

parser = argparse.ArgumentParser(description='Generate certificates for VMs.')
parser.add_argument('--vms-config', default = 'vm_names.json', help='path to json VM configuration')
parser.add_argument('--intermediate-cert', default = 'intermediate_ca.pem', help='path to intermediate CA certificate')
parser.add_argument('--intermediate-key', default = 'intermediate_ca.key', help='path to intermediate CA key')
args = parser.parse_args()

vm_names_json_filename = args.vms_config
intermediate_cert_name = args.intermediate_cert
intermediate_key_name = args.intermediate_key

cfg = Config()
kvwrapper = KeyVaultWrapper(
    client_id = cfg.client_id, 
    secret = cfg.secret,
    tenant_id = cfg.tenant_id)

openssl = OpenSSLWrapper()

ca_cert = openssl.load_certificate(intermediate_cert_name)
sign_key = openssl.load_key(intermediate_key_name)

with open(vm_names_json_filename, "rt") as f:
    vm_list = json.loads(f.read())

    for vm in vm_list:
        key = openssl.generate_key(2048)
        cert = openssl.generate_cert(vm["vmName"], key, ca_cert, sign_key)
        pfx = kvwrapper.generate_pfx(
            cert,
            key,
            '')
        kvwrapper.upload_pfx(
            vault_uri = cfg.vault_uri, 
            cert_name = vm["vmName"], 
            pfx_bytes = pfx)

#        openssl.dump_certificate(cert, "new.pem")
#        print(openssl.verify_cert_chain(["root_ca.pem", "intermediate_ca.pem"], "new.pem"))