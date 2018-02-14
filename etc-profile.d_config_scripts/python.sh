#
# python configuration for bash
#
# by: Max Novelli
#     2012-09-25
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="/usr/local/pkg/python/2.7.3/bin:${PATH}"
  export PATH

  # alias
  alias python=/usr/local/pkg/python/2.7.3/bin/python

fi

