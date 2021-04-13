#!/bin/sh -e

SERVICE="http://kafkalogseq"

until curl -fs ${SERVICE}  > /dev/null; 
do
  >&2 echo "${SERVICE} is unavailable - sleeping"
  sleep 2
done
>&2 echo "${SERVICE} is up"

/bin/seqcli/seqcli apikey create --title='kafka-docker-api' --token=${SEQ_LOG_TOKEN} --server=${SERVICE}

exit 0
