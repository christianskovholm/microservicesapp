#!/bin/bash

/etc/psql-migrate postgres idea_service db_tables.sql

tmp=/tmp/main.pid
main=$GOPATH/bin/ideaservice

build() {
    make > /dev/null
    build_exit_code=$?
    tmp=/tmp/main.pid

    if [ -f $tmp ]
    then
        kill $(cat $tmp)
        rm -f $tmp
    fi

    if [ $build_exit_code != 0 ]
    then
        echo Please fix the above error to restart the hot reloading process.
    else
        exec $main &
        pidof $main >| $tmp
    fi
}

lockBuild() {
  if [ -f /tmp/main.lock ]
  then
    inotifywait -e delete /tmp/main.lock
  fi
  touch /tmp/main.lock
}

unlockBuild() {
  rm -f /tmp/main.lock
}

if [ -f $tmp ]
then
    rm -f $tmp
fi

build

inotifywait --quiet -e close_write -r -m ./ | 
  while read path action file; do
      ext="${file#*.}"
      if [ $ext == "go" ]
      then
        echo "Registered a file change in $file. Rebuilding."
        lockBuild
        build
        unlockBuild
      fi
  done