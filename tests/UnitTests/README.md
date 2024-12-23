# Geopack-2008 test data
## Sources
Find the single precision version of GEOPACK-2008 [here](https://geo.phys.spbu.ru/~tsyganenko/Geopack-2008.html).
## Install Intel Fortran compiler (Linux, Debian-based)
```shell
sudo apt install -y gpg-agent wget
wget -O- https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB | gpg --dearmor | sudo tee /usr/share/keyrings/oneapi-archive-keyring.gpg > /dev/null
echo "deb [signed-by=/usr/share/keyrings/oneapi-archive-keyring.gpg] https://apt.repos.intel.com/oneapi all main" | sudo tee /etc/apt/sources.list.d/oneAPI.list
sudo apt update
sudo apt install intel-oneapi-compiler-fortran
```
In order to set up the environment variables, run the following command (current shell):
```shell
source /opt/intel/oneapi/setvars.sh
```
To set up the environment variables permanently, run the following command:
```shell
nano ~/.bashrc
```
Insert in the end of the file:
```shell
source /opt/intel/oneapi/setvars.sh
```
## Compile, build binary, generate data (Linux)
To compile and build binary (executable), follow these steps:
- cd /path/to/Geopack-2008 and T96 sources;
- Get the author's example code [here](https://geo.phys.spbu.ru/~tsyganenko/models/Examples_1_2.for);
- execute in shell:
```shell
ifX -O3 Geopack-2008.for T96.for <Example_code.for> -o gen_data && ./gen_data
```
