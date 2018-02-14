#
# fidl configuration for bash
#
# by: Max Novelli
#     2011-04-14
#
# 2011-05-04 - J. Tremel - added refdir, release, and ld_library_path, and path
# 2011-07-14 - Max Novelli - updated path to use new version of fidl
# 2012-11-05 -JT - updated to new version of fidl

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  #PATH="${PATH}:/usr/local/pkg/fidl_2.64/scripts:/usr/local/pkg/fidl/bin:/usr/local/pkg/nil-tools/bin"
  PATH="${PATH}:/usr/local/pkg/fidl/scripts:/usr/local/pkg/fidl/bin:/usr/local/pkg/nil-tools/bin"
  export PATH

  # fidl env vars
  export REFDIR=/usr/local/pkg/fidl/lib
  export RELEASE=/usr/local/pkg/nil-tools/bin
  export LD_LIBRARY_PATH=/usr/lib:/usr/local/lib

  # alias
  alias fidl="/usr/local/pkg/fidl/scripts/fidl -vm"

fi

