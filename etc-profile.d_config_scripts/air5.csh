#
# air 5 configuration for csh
#
# by: Max Novelli
#     2011-04-14
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/AIR-5.3.0/bin"

endif

