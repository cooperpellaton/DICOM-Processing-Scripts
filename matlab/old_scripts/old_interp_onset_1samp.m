%Computes onset time with ttests for each condition. The onset time is defined as the time point (xi) at which the time course
%magnitudes across all subjects in a given condition rise above a baseline (defined as the average of the 
%first and last time points). One array is the magnitudes at point xi of each subject, the other
%is baseline of each subject.

%NOTE: This is different from 'interp_onset_2samp.m'

% J. Tremel (tremeljosh@gmail.com), 2009, University of Pittsburgh

clear all;

%define file names of region time courses (rows are time points 1:16 and columns are conditions
%divided into subjects [i.e., S1Cond1 S2Cond1 S3Cond1 ... etc. would be the column headers]);
infile = {'R01.xls'; 'R02.xls'; 'R03.xls'; 'R04.xls'; 'R05.xls'; 'R06.xls'; 'R07.xls';...
    'R08.xls'; 'R09.xls'; 'R10.xls'; 'R11.xls'; 'R12.xls'; 'R13.xls'; 'R14.xls';...
    'R15.xls'; 'R16.xls'; 'R17.xls'; 'R18.xls'; 'R19.xls'; 'R20.xls'; 'R21.xls';... 
    'R22.xls'; 'R23.xls'; 'R24.xls'; 'R25.xls'; 'R26.xls'; 'R27.xls'; 'R28.xls';...
    'R29.xls'; 'R30.xls'; 'R31.xls'; 'R32.xls'; 'R33.xls'; 'R34.xls'; 'R35.xls';...
    'R37.xls'; 'R38.xls';};
% 'R36.xls'; 

% infile = {'R01_norm.xls'; 'R02_norm.xls'; 'R03_norm.xls'; 'R04_norm.xls'; 'R05_norm.xls'; 'R06_norm.xls'; 'R07_norm.xls';...
%     'R08_norm.xls'; 'R09_norm.xls'; 'R10_norm.xls'; 'R11_norm.xls'; 'R12_norm.xls'; 'R13_norm.xls'; 'R14_norm.xls';...
%     'R15_norm.xls'; 'R16_norm.xls'; 'R17_norm.xls'; 'R18_norm.xls'; 'R19_norm.xls'; 'R20_norm.xls'; 'R21_norm.xls';... 
%     'R22_norm.xls'; 'R23_norm.xls'; 'R24_norm.xls'; 'R25_norm.xls'; 'R26_norm.xls'; 'R27_norm.xls'; 'R28_norm.xls';...
%     'R29_norm.xls'; 'R30_norm.xls'; 'R31_norm.xls'; 'R32_norm.xls'; 'R33_norm.xls'; 'R34_norm.xls'; 'R35_norm.xls';...
%     'R36_norm.xls'; 'R37_norm.xls'; 'R38_norm.xls';};


%Specify format of region files (.xls or .txt)
FileForm='.xls';
%Specify text output file name
FileName='output_onset_1samp.txt';
%define number of regions;
nreg=37;
%Define number of conditions;
ncond=5;
%Define number of subjects;
nsubj=14;
%Define number of columns in infile
ncol=ncond*nsubj;
%Define number of time points in the time course
ntp=16;
tp=1:ntp;
nxi=(ntp*1000)-999;
%Define how many points between each tp to interpolate
ntpi=1000;
tpi=1/ntpi;
%Define minimum timepoint for an onset of signal (1=beginning)
min_O=1;
min_O=(min_O*1000)-999;
%Define maximum timepoint for an onset of signal (ntp=end)
max_O=ntp;
max_O=(max_O*1000)-999;
%Define Significance level for ttest (default p=0.05)
tSig=0.05;
%Define Significance level for anova (default p=0.05)
aSig=0.05;

S=1:ncol;                                   %Data array of n columns
Sub=1:nsubj;                                %Data array of n subjects
rgn=1;

output(1:nreg,1:(ncond+1))=0;

%loop for all regions (infiles)

for x=1:nreg
    rgn
    %Preallocate arrays and variables
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

    %SMOOTH TCs with a Savitzky-Golay filter
%     for S=1:ncol;
%         stc(:,S)=smooth(tc(:,S),'sgolay');
%     end
    S=1:ncol;
    %interpolate
    yi(:,S)=interp1(tc(:,S),xi,'linear');



    %COMPUTE ONSET TIMES
        Sub=1:nsubj;
        while On_t(Sub)==0;
            %Baseline array as average of 1st and last time point
%             base(Sub)=mean([tc(1,Sub); tc(ntp,Sub)]);
            base(Sub) = 0;
            xi=min_O;
            
            while xi~=0 && xi < max_O && ttest2(base(Sub),yi(xi,Sub), tSig)==0;
                xi=xi+1;
            end
            
            On_m(S)=yi(xi,S);
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
                output(rgn,b)=On_t(bsub)*2;
                if b==ncond
                    b=0;
                else
                    b=b+1;
                    bsub=bsub+nsubj;
                end
            end

        rgn=rgn+1;
end
save(FileName,'output','-ASCII')
output
FileName