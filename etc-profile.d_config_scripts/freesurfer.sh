#
# freesurfer configuration for bash
#
# by: Max Novelli
#     2011-11-29
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # freesurfer home 
  FREESURFER_HOME="/usr/local/pkg/freesurfer-Linux-centos4_x86_64-stable-pub-v5.1.0"
  export FREESURFER_HOME

  # path
  #PATH="${PATH}:${FREESURFER_HOME}/bin"
  #export PATH

  # setup
  source ${FREESURFER_HOME}/SetUpFreeSurfer.sh >/dev/null

fi

