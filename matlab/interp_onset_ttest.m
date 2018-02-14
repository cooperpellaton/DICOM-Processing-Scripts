%Computes onset time with ttests for each condition. The onset time is defined as the time point (xi) at which the time course
%magnitudes across all subjects in a given condition rise above a baseline (defined as the average of the 
%first and last time points). One array is the magnitudes at point xi of each subject, the other
%is baseline of each subject. There is also an option to use a one sample t-test and assume a
%baseline of zero.

%(There's also apparently something at the bottom which will output the peak times and
%magnitudes...)

%Lastly, this is some pretty terrible code... Someday I'll rewrite it to be nice, clear, 
%neat, and efficient...

% J. Tremel (tremeljosh@gmail.com), 2009, University of Pittsburgh

clear all;

%define file names of region time courses (rows are time points [e.g., 1:16] and columns are conditions
%divided into subjects [i.e., S1Cond1 S2Cond1 S3Cond1 ... etc. would be the column headers]);
infile = {'R01.xls'; 'R02.xls'; 'R03.xls'; 'R04.xls'; 'R05.xls'; 'R06.xls'; 'R07.xls';...
    'R08.xls'; 'R09.xls'; 'R10.xls'; 'R11.xls'; 'R12.xls'; 'R13.xls'; 'R14.xls';...
    'R15.xls'; 'R16.xls'; 'R17.xls'; 'R18.xls'; 'R19.xls'; 'R20.xls'; 'R21.xls';... 
    'R22.xls'; 'R23.xls'; 'R24.xls'; 'R25.xls'; 'R26.xls'; 'R27.xls'; 'R28.xls';...
    'R29.xls'; 'R30.xls'; 'R31.xls'; 'R32.xls'; 'R33.xls'; 'R34.xls';};

% Specify format of INPUT files (.xls or .txt):
FileForm = '.xls';

% Specify text OUTPUT file name:
FileName = 'output_onsets_peaks_alpha05_test_stand&smloess.txt';

% Number of regions:
nreg = 34;

% Number of conditions:
ncond = 5;

% Number of subjects:
nsubj = 16;

% Number of time points of the timecourses:
ntp = 16;

% How many points between each tp to interpolate:
ntpi = 1000;

% Define minimum timepoint for an onset of signal (1=beginning)
min_O = 1;

% Define maximum timepoint for an onset of signal (ntp=end)
max_O = ntp;

% Significance level for ttests
tSig = 0.01;

% Choose baseline to test against: ('z' [zero] is baseline of 0, 'e' [estimate] is a noise
% estimate taking the average of the first and last timepoints)
base_test = 'e';

% Smooth data? ('y' or 'n')
smoothq = 'n';
% Smoothing algorithm: ('sgolay', 'moving', 'lowess', 'loess', 'rlowess', 'rloess')
smalgo = 'sgolay';


%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%
%~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~%

ncol=ncond*nsubj;
tp=1:ntp;
tpi=1/ntpi;
nxi=(ntp*1000)-999;
min_O=(min_O*1000)-999;
max_O=(max_O*1000)-999;

S=1:ncol;
Sub=1:nsubj;
rgn=1;

output(1:nreg,1:(ncond+1))=0;

%loop for all regions (infiles)

for x=1:nreg
    rgn
    %Preallocate
    clear On_t
    clear On_m
    clear base
    clear Div_t
    clear ArrayC
    On_t(ncol)=0;                           %Onset Time
    On_m(ncol)=0;                           %Magnitude at onset time
    base(ncol)=0;                           %Baseline for each subject
    yi(nxi,:)=0;                            %interpolated time course magnitudes (16000 pts total)
    xi=1:tpi:ntp;                           %interpolated time point (16000 points total)
    Div_t(1)=0;                             %time point of divergence
    
    %import data from specified format
    if FileForm=='.xls'
        [ndata]=xlsread (infile{x});
    elseif FileForm=='.txt'
        [ndata]=textread (infile{x});
    else
    end

    %set up time course matrix
    S=1:ncol;
    xi=1:tpi:ntp;
    tc(:,S)=ndata(:,S+1);

        if smoothq == 'y'
            for S=1:ncol
                stc(:,S)=smooth(tc(:,S),smalgo);
            end
            S = 1:ncol;
            yi(:,S) = interp1(stc(:,S),xi,'linear');
        else
            S=1:ncol;
            yi(:,S)=interp1(tc(:,S),xi,'linear');
        end

    %COMPUTE ONSET TIMES
        Sub=1:nsubj;
        while On_t(Sub)==0;
            %Baseline array as average of 1st and last time point

            if smoothq == 'y'
                if base_test == 'e'
%                     base(Sub)=mean([stc(1,Sub); stc(ntp,Sub)]);
                    base(Sub) = (stc(1,Sub));
                elseif base_test == 'z'
                    base(Sub) = 0;
                else
                end
            else
                if base_test == 'e'
                    base(Sub) = (tc(1,Sub));
%                     base(Sub)=mean([tc(1,Sub); tc(ntp,Sub)]);
                elseif base_test == 'z'
                    base(Sub) = 0;
                else
                end
            end
            
            xi=min_O;
            
            while xi~=0 && xi < max_O && ttest2(base(Sub),yi(xi,Sub), tSig)==0;
                xi=xi+3;
            end


            On_m(Sub)=mean(yi(xi,Sub));
            On_t(Sub)=((xi+999)/1000);
            
            if Sub < ncol
                Sub=Sub+nsubj;
            else
            end
        end
        %Set output for onset times
            b=1;
            bsub=1;
            while b~=0
                output(rgn,b)=On_t(bsub);
                if b==ncond
                    b=0;
                else
                    b=b+1;
                    bsub=bsub+nsubj;
                end
            end
            b = 1;
            bsub = 1;
            while b ~=0
                output2(rgn,b)=On_m(bsub);
                if b == ncond
                    b = 0;
                else
                    b = b + 1;
                    bsub = bsub + nsubj;
                end
            end
            
            
    %COMPUTE PEAK TIMES/MAGNITUDES
    Sub = 1;
    cond = 1;
    col = 1;
    while col ~= 0
        [pk_m{rgn}(Sub,cond), pk_t{rgn}(Sub,cond)] = max(yi(:,col));
        pk_t{rgn}(Sub,cond) = ((pk_t{rgn}(Sub,cond)+999)/1000);
        if Sub == nsubj 
            cond = cond + 1;
            Sub = 1;
        else
            Sub = Sub + 1;
        end
        if col == ncol
            col = 0;
        else
            col = col + 1;
        end
    end
    
        rgn=rgn+1;
end
save(FileName,'output','-ASCII')
output
FileName