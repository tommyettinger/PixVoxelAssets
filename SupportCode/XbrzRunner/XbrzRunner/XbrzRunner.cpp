// XbrzRunner.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <fstream>
#include <iostream>

using namespace xbrz;
using namespace std;

const int scaling = 3;
int main(int argc, char* argv[])
{
	if (argc != 3)
		return -1;
	
	streampos size;
	char * memblock;
	char * outmem;

	fstream file("in.bin", ios::in | ios::binary | ios::ate);
	if (file.is_open())
	{
		printf("XBRZ processing in.bin");
		size = file.tellg();
		memblock = new char[size];
		outmem = new char[size * scaling * scaling];
		file.seekg(0, ios::beg);
		file.read(memblock, size);
		file.close();

		uint32_t * pixblock = new uint32_t[size / 4];
		uint32_t * outblock = new uint32_t[(size / 4) * scaling * scaling];

		for (int i = 0; i < size / 4; i++)
		{
			pixblock[i] = (memblock[i * 4 + 0] << 24) | ((memblock[i * 4 + 1] << 16) & 0xff0000) | ((memblock[i * 4 + 2] << 8) & 0xff00) | ((memblock[i * 4 + 3]) & 0xff);
		}

		scale(scaling, pixblock, outblock, atoi(argv[1]), atoi(argv[2]), ColorFormat::ARGB);

		for (int i = 0; i < (size / 4) * scaling * scaling; i++)
		{
			outmem[i * 4 + 0] =  outblock[i] >> 24;
			outmem[i * 4 + 1] = (outblock[i] >> 16) & 0xff;
			outmem[i * 4 + 2] = (outblock[i] >> 8)  & 0xff;
			outmem[i * 4 + 3] = (outblock[i])       & 0xff;
		}
		file.close();
		

		fstream ofile("out.bin", ios::out | ios::binary |ios::trunc);
		if (ofile.is_open())
		{
			printf("XBRZ processing out.bin");
			ofile.seekg(0, ios::beg);

			ofile.write(outmem, size * scaling * scaling);

			ofile.close();
		}
		
		delete[] memblock;
		delete[] pixblock;
		delete[] outblock;
		delete[] outmem;
	}
	else return -1;
    return 0;
}

