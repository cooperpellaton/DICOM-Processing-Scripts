#
# mono configuration for bash
#
# by: Max Novelli
#     2012-03-28
#

uid=`id -u`
if [ $uid -ne 0 -a -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # mono version
  MONO_VER="2.11"
  export MONO_VER

  # path
  PATH="${PATH}:/usr/local/pkg/mono-${MONO_VER}/bin"
  export PATH

  # library path
  LD_LIBRARY_PATH="${LD_LIBRARY_PATH}:/usr/local/pkg/mono-${MONO_VER}/lib"
  export LD_LIBRARY_PATH

fi

