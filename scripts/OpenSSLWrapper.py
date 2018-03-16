import base64
from OpenSSL import crypto

class OpenSSLWrapper(object):
    def load_key(self, filename):
        with open(filename, "rt") as f:
            return crypto.load_privatekey(crypto.FILETYPE_PEM, f.read())

    def load_certificate(self, filename):
        with open(filename, "rt") as f:
            return crypto.load_certificate(crypto.FILETYPE_PEM, f.read())

    def dump_key(self, key, filename):
        with open(filename, "wb+") as f:
            f.write(crypto.dump_privatekey(crypto.FILETYPE_PEM, key))

    def dump_certificate(self, cert, filename):
        with open(filename, "wb+") as f:
            f.write(crypto.dump_certificate(crypto.FILETYPE_PEM, cert))

    def generate_key(self, length = 4096):
        key =  crypto.PKey()
        key.generate_key(crypto.TYPE_RSA, length)
        return key

    def generate_cert(selft, cn, key, cacert = None, sign_key = None):
        cert = crypto.X509()
        cert.get_subject().C = "US"
        cert.get_subject().ST = "Washington"
        cert.get_subject().L = "Redmond"
        cert.get_subject().O = "Contoso"
        cert.get_subject().OU = "Contoso"
        cert.get_subject().CN = cn
        cert.set_serial_number(1000)
        cert.gmtime_adj_notBefore(0)
        cert.gmtime_adj_notAfter(10 * 365 * 24 * 60 * 60)
        cert.set_pubkey(key)
        if cacert and sign_key:
            cert.set_issuer(cacert.get_subject())
            cert.sign(sign_key, 'sha1')
        else:
            cert.set_issuer(cert.get_subject())
            cert.sign(key, 'sha1')

        return cert
        
    def generate_pfx(self, certificate, key, passphrase):
        p12 = crypto.PKCS12()
        p12.set_certificate(certificate)
        p12.set_privatekey(key)
        return p12.export()
    
    def verify_cert_chain(self, ca_certificates, cert):
        store = crypto.X509Store()
        for ca in ca_certificates:
            with open(ca, "rt") as f:
                store.add_cert(crypto.load_certificate(crypto.FILETYPE_PEM, f.read()))

        with open(cert, "rt") as f:
            ctx = crypto.X509StoreContext(store, crypto.load_certificate(crypto.FILETYPE_PEM, f.read()))

        try:
            ctx.verify_certificate()
        except Exception as e:
            print(e)
            return False
        return True
