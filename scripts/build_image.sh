#!/bin/bash

cd ../src/Identity/Edc.Identity.WebApi/
docker build -f Dockerfile  . -t identity:latest 
