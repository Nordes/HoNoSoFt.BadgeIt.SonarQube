# From
# https://medium.com/travis-on-docker/how-to-version-your-docker-images-1d5c577ebf54
set -ex
# SET THE FOLLOWING VARIABLES
# docker hub username
USERNAME=honosoft
# image name
IMAGE=badgeit
docker build -t $USERNAME/$IMAGE:latest .
