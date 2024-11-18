#!/bin/bash
# https://stackoverflow.com/questions/3455625/linux-command-to-print-directory-structure-in-the-form-of-a-tree

find . | sed -e "s/[^-][^\/]*\//  |/g" -e "s/|\([^ ]\)/+ \1/"