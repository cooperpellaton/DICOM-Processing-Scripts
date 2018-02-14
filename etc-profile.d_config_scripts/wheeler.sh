#
# wheeler custome configuration for bash
#
# by: Max Novelli
#     2011-04-14
#


uid=`id -u`
if [ $uid -ne 0 ] && [ -n "$BASH_VERSION" -o -n "$KSH_VERSION" -o -n "$ZSH_VERSION" ] && [ ${TERM} != "dumb" ]; then

  # path
  PATH="${PATH}:/usr/local/pkg/fidl_preprocess:/usr/local/pkg/DifMod/bin:/usr/local/pkg/itksnap/bin:/usr/local/pkg/Slicer3.6/"
  export PATH
  
  alias difmod='mono /usr/local/pkg/DifMod/bin/difmod.exe'
  
fi

