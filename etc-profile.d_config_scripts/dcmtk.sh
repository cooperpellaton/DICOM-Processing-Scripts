#
# dcmtk configuration for bash
#
# by: Max Novelli
#     2011-04-14
#

uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/dcmtk-3.6.0/bin"
  export PATH

  # afni plugin
  DCMDICTPATH="/usr/local/pkg/dcmtk-3.6.0/share/dcmtk/dicom.dic"
  export DCMDICTPATH

fi

