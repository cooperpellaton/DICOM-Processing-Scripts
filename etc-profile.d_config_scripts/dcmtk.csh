#
# dcmtk configuration for csh
#
# by: Max Novelli
#     2011-04-14
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/dcmtk-3.6.0/bin"

  # dicom dictionary path 
  setenv DCMDICTPATH "/usr/local/pkg/dcmtk-3.6.0/share/dcmtk/dicom.dic"

endif

