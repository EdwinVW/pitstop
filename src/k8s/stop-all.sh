#!/bin/bash
kubectl delete svc -l app=pitstop -n pitstop
kubectl delete deploy -l app=pitstop -n pitstop