#
# nil configuration for csh
#
# by: Max Novelli
#     2011-05-05
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/nil.20110503/bin"

endif

