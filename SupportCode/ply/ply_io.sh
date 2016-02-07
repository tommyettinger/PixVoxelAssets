#!/bin/bash
#
cp ply_io.h /$HOME/include
#
gcc -c -I/$HOME/include ply_io.c
if [ $? -ne 0 ]; then
  echo "Errors compiling ply_io.c."
  exit
fi
#
mv ply_io.o ~/libc/$ARCH/ply_io.o
#
echo "Library installed as ~/libc/$ARCH/ply_io.o"
