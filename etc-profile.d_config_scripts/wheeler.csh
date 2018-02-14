#
# wheeler custom configuration for csh
#
# by: Max Novelli
#     2011-10-11
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/fidl_preprocess"
  
  
endif

