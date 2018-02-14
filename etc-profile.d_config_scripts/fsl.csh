#
# fsl configuration for csh
#
# by: Max Novelli
#     2011-04-14
#

set uid=`id -u`

if ( $uid != 0 && $TERM != "dumb" ) then

  # path
  setenv PATH "${PATH}:/usr/local/pkg/fsl/bin"

  # fsl dir
  setenv FSLDIR "/usr/local/pkg/fsl"

  # fsl configuration
  source ${FSLDIR}/etc/fslconf/fsl.csh

endif

