FROM centos:7

RUN yum -y update && \
    yum -y install openssl && \
    yum -y install epel-release && \
    yum -y install python-pip

RUN pip install azure-keyvault && \
    pip install pyopenssl

WORKDIR /usr/src/app