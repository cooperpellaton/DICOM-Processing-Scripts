#
# afni configuration for bash
#
# by: Max Novelli
#     2009-08-17
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/afni"
  export PATH

  # afni plugin
  AFNI_PLUGINPATH="/usr/local/pkg/afni"
  export AFNI_PLUGINPATH

fi

