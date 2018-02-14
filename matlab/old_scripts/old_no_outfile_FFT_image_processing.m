% J. Tremel (tremeljosh@gmail.com), 2009, University of Pittsburgh

clear all

% cd to dir to process
cd /Users/josh/Desktop/FvH_Stimuli

% Number of images to process (considering filenames are numbered consecutively)
nImg = 85;

% number of first image
firstImg = 1;

% Noise percentage to introduce into image
nsp = 0.35;

% Number of images to make for each face/house
    % e.g. @ 15 fps, 90 iters = 6 s of video
niter = 90;

% Economy: 1 removes intermediates
econ = 1;

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~~~~~~~~~~~~~~~~~~~~ SCRIPT BELOW ~~~~~~~~~~~~~~~~~~~~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

nhundred = nsp*100;
subdir = sprintf('%i_percent_noise',nhundred);

% calculate needed SNR in dB
sgp = 1 - nsp;          % Signal %%
spn = sgp/nsp;          % Signal %%/Noise %%
snr = 20*log10(spn);    % dB conversion
iter = 1


while iter <= niter
    iImg = firstImg;
    while iImg <= nImg

        %if iImg is a single digit, pad with a zero
        
        filenm = sprintf('%i.bmp', iImg);


        rawimg{iImg} = imread(filenm);
        imglength = length(rawimg{iImg});
        padto = pow2(nextpow2(imglength));

        % take forward 2D-FFT of img and store raw FFT values into matrices for each img.
        FT{iImg}{iter} = fft2(im2double(rawimg{iImg}), padto, padto);
        FT_ampli{iImg}{iter} = abs(FT{iImg}{iter});
%         FT_power{iImg}{iter} = FT{iImg}{iter}.*conj(FT{iImg}{iter})/padto;
        FT_phase{iImg}{iter} = unwrap(angle(FT{iImg}{iter}));
        iImg = iImg + 1;
    end

    if econ == 1
        clear rawimg
        clear FT
    else
    end
    
    % create mean amplitude array
    iImg = firstImg;
    while iImg <= nImg
        if iImg == 1
            FT_ampli_mean = FT_ampli{iImg}{iter};
            iImg = iImg + 1;
        else
            FT_ampli_mean = FT_ampli_mean + FT_ampli{iImg}{iter};
            iImg = iImg + 1;
        end
    end

    FT_ampli_mean = FT_ampli_mean./nImg;

    % Make noise matrix and convolve w/ phase matrices
    iImg = firstImg;
    while iImg <= nImg
        FT_phase_wnoise{iImg}{iter} = awgn(FT_phase{iImg}{iter},snr);
        iImg = iImg + 1;
    end


    % Rebuild FFT signal and invert, remove round off error, cut pad back to original size
    iImg = firstImg;
    while iImg <= nImg
        FT_recomb{iImg}{iter} = (FT_ampli_mean.*exp(1i*FT_phase_wnoise{iImg}{iter}));
        procImg{iImg}{iter} = real(ifft2(FT_recomb{iImg}{iter}));
        finImg{iImg}{iter} = procImg{iImg}{iter}(1:imglength,1:imglength);
        iImg = iImg + 1;
    end

    if econ == 1
        clear FT_recomb
        clear procImg
    else
    end
    
    if iter < niter
        iter = iter + 1
    else
        iter = iter + 1;
    end
end


