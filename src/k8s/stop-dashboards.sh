#!/bin/bash
kill $(ps aux | grep 'kubectl' | awk '{print $1}')