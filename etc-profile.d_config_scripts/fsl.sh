#
# fsl configuration for bash
#
# by: Max Novelli
#     2011-04-14
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/fsl/bin"
  export PATH

  # fsl dir
  FSLDIR="/usr/local/pkg/fsl"
  export FSLDIR

  # fsl configuration
  source ${FSLDIR}/etc/fslconf/fsl.sh

fi

