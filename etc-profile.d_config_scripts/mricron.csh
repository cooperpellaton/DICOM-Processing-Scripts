#
# mricron configuration for csh
#
# by: Max Novelli
#     2009-08-17
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/mricron.20110619"

endif

