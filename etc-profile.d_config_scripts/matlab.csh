#
# matlab configuration for csh
#
# by: Max Novelli
#     2011-04-14
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/MATLAB/R2011a/bin/"

endif

