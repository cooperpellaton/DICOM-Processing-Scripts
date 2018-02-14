#
# nil configuration for bash
#
# by: Max Novelli
#     2011-05-03
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/nil.20110503/bin"
  export PATH

fi

