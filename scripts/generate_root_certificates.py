from config import Config
from OpenSSLWrapper import OpenSSLWrapper
from KeyVaultWrapper import KeyVaultWrapper

cfg = Config()
kvwrapper = KeyVaultWrapper(
    client_id = cfg.client_id, 
    secret = cfg.secret,
    tenant_id = cfg.tenant_id)

root_secret_name = "root-ca"
intermediate_secret_name = "intermediate-ca"
root_ca_name = "root_ca.pem"
root_key_name = "root_ca.key"
intermediate_cert_name = "intermediate_ca.pem"
intermediate_key_name = "intermediate_ca.key"

openssl = OpenSSLWrapper()

root_key = openssl.generate_key(4096)
root_cert = openssl.generate_cert("example_root.com", root_key)
openssl.dump_key(root_key, root_key_name)
openssl.dump_certificate(root_cert, root_ca_name)

intermediate_key = openssl.generate_key(2048)
intermediate_cert = openssl.generate_cert("example_intermediate.com", intermediate_key, root_cert, root_key)
openssl.dump_key(intermediate_key, intermediate_key_name)
openssl.dump_certificate(intermediate_cert, intermediate_cert_name)

kvwrapper.upload_secret(cfg.vault_uri, root_secret_name, root_ca_name)
kvwrapper.upload_secret(cfg.vault_uri, intermediate_secret_name, intermediate_cert_name)

if openssl.verify_cert_chain([root_ca_name], intermediate_cert_name):
    print("Root and intermediate certificates were created.")
else:
    print("Root and intermediate certificates creation failed.")
    