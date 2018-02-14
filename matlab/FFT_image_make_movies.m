% This script should be run after 'FFT_image_processing_iters_forea_image_savemat.m' to 
% generate movies from the .mat files. Make sure the .mat files are open before you run 
% this. This will save .avi files (6 s movies are about 40-50 MB each). 
%
% Really annoying: you cannot do anything on the computer while this is running. Make
% sure you turn off the screensaver and don't let the computer sleep. This will open a 
% bunch of windows, and if there is anything on top of the windows when they open, it 
% will show up in the movies

% J. Tremel (tremeljosh@gmail.com), 2010, University of Pittsburgh
%       Revised March 2011

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% User options:
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
    % Frame-rate (in frames/second, default = 15 fps):
    fps = 15;
    
    % Length of video (in seconds):
    vidlength = 6;
    
    % Quality  (number between 0-100, default = 75):
    qual = 75;
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% SCRIPT:
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
% This is temporary (compatibility for old versions):
if exist('niter','var') == 1
    nframes = niter;
else
end

subdir = sprintf('%s_percent_noise',num2str(nhundred));

% Check for output dir
if exist(subdir,'dir') == 0
    mkdir(subdir);
else
end

% Check that nframes is correct for the frame rate and video length:
nframes_new = times(fps,vidlength);
if nframes ~= nframes_new
    disp(['Number of frames to be used in video: ',num2str(round(nframes_new))])
    nframes = round(nframes_new);
    fps_new = rdivide(nframes,vidlength);
    fps = fps_new;
    disp(['New framerate: ',num2str(fps)])
else
end
clear fps_new
clear nframes_new

break

iImg = firstImg;
while iImg <= nImg
    clear filename
    clear facemov
    
    disp(['processing image ',num2str(Img)])

    iImg = sprintf('%02d %i',iImg);

        filename = sprintf('./%s/image%s_%s_percent_noise.avi',subdir,num2str(iImg)...
            ,num2str(nhundred));
        facemov = avifile(filename,'fps',fps,'quality',qual);

    iter = 1;
    for iter = 1:nframes
        imagesc(finImg{iImg}{iter}); colormap gray
        F = getframe;
        facemov = addframe(facemov, F);
    end
    close Figure 1
    facemov = close(facemov);
    disp(['finished processing image ',num2str(iImg)])
    iImg = iImg + 1;
end