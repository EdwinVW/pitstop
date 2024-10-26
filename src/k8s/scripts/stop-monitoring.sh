#!/bin/bash
kubectl delete svc --all -n monitoring
kubectl delete deploy --all -n monitoring
kubectl delete virtualservice --all -n monitoring
kubectl delete destinationrule --all -n monitoring