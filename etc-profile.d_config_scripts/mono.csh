#
# mono configuration for bash
#
# by: Max Novelli
#     2012-03-28
#

set uid=`id -u`

if ( $uid != 0 ) then

  # mono version
  setenv MONO_VER "2.11"

  # path
  setenv PATH "${PATH}:/usr/local/pkg/mono-${MONO_VER}/bin"

  # library path
  setenv LD_LIBRARY_PATH "${LD_LIBRARY_PATH}:/usr/local/pkg/mono-${MONO_VER}/lib"

endif
