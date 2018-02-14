#
# freesurfer configuration for csh
#
# by: Max Novelli
#     2011-11-29
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # freesurfer home 
  setenv FREESURFER_HOME "/usr/local/pkg/freesurfer-Linux-centos4_x86_64-stable-pub-v5.1.0"

  # path
  #setenv PATH "${PATH}:${FREESURFER_HOME}/bin"

  # setup
  source ${FREESURFER_HOME}/SetUpFreeSurfer.csh

endif

