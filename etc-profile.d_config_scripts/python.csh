#
# python configuration for csh
#
# by: Max Novelli
#     2012-09-25
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "/usr/local/pkg/python/2.7.3/bin:${PATH}"
  export PATH

  # alias
  alias python "/usr/local/pkg/python/2.7.3/bin/python"

endif

