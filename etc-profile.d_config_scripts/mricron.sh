#
# mricron configuration for bash
#
# by: Max Novelli
#     2009-08-17
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/mricron.20110619"
  export PATH

fi

