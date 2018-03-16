from azure.keyvault import KeyVaultClient
from azure.common.credentials import ServicePrincipalCredentials

import base64
from OpenSSL import crypto

class KeyVaultWrapper(object):
    def __init__(self, client_id, secret, tenant_id):
        credentials = ServicePrincipalCredentials(
                                    client_id = client_id,
                                    secret = secret,
                                    tenant = tenant_id)
        self.kvclient = KeyVaultClient(credentials)

    def upload_secret(self, vault_uri, secret_name, filename):
        with open(filename, "rt") as f:
            self.kvclient.set_secret(vault_uri, secret_name, f.read())

    def get_secret(self, vault_uri, secret_name, secret_version):
        return self.kvclient.get_secret(vault_uri, secret_name, secret_version).value

    def get_secret_to_file(self, vault_uri, secret_name, secret_version, filename):
        with open(filename, 'wt+') as file:
            file.write(self.get_secret(vault_uri, secret_name, secret_version))

    def get_certificate_and_key(self, vault_uri, cert_name, cert_version):
        secret = self.get_secret(vault_uri, cert_name, cert_version)
        p12 = crypto.load_pkcs12(base64.b64decode(secret), "")
        return [p12.get_certificate(), p12.get_privatekey()]

    def get_certificate_and_key_to_file(self, vault_uri, cert_name, cert_version, key_filename, cert_filename):
        [cert, key] = self.get_certificate_and_key(vault_uri, cert_name, cert_version)

        with open(cert_filename, 'wb+') as pem_file:
            pem_file.write(crypto.dump_certificate(crypto.FILETYPE_PEM, cert))

        with open(key_filename, 'wb+') as key_file:
            key_file.write(crypto.dump_privatekey(crypto.FILETYPE_PEM, key))

    def upload_pfx(self, vault_uri, cert_name, pfx_bytes):
        encoded = base64.b64encode(pfx_bytes)
        self.kvclient.import_certificate(
            vault_base_url = vault_uri,
            certificate_name = cert_name,
            base64_encoded_certificate = encoded.decode('utf-8'))

    def generate_pfx(self, certificate, key, passphrase):
        p12 = crypto.PKCS12()
        p12.set_certificate(certificate)
        p12.set_privatekey(key)
        return p12.export()