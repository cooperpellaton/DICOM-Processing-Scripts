#
# fidl configuration for csh
#
# by: Max Novelli
#     2011-04-14
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/fidl/scripts:/usr/local/pkg/fidl/bin:/usr/local/pkg/nil-tools/bin"

  # fidl env vars
  setenv REFDIR /usr/local/pkg/fidl/lib
  setenv RELEASE /usr/local/pkg/nil-tools/bin
  setenv LD_LIBRARY_PATH /usr/lib:/usr/local/lib

  # alias
  alias fidl "/usr/local/pkg/fidl/scripts/fidl -vm"

endif

