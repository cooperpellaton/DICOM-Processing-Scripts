% interpolation for time courses, average per region (i.e., not single subject).

clear all

%Set up arrays
    tp = [1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16]';
    
%     infile = {'ROI01_-27_-92_-02.xls'; 'ROI02_26_-91_05.xls'; 'ROI03_29_-89_-05.xls'; 'ROI04_17_-93_-06.xls';...
%     'ROI05_-26_-87_-13.xls'; 'ROI06_-17_-96_-10.xls'; 'ROI07_45_-64_-07.xls'; 'ROI08_-39_-81_-11.xls';...
%     'ROI09_42_-75_-07.xls'; 'ROI10_34_-82_18.xls'; 'ROI11_29_-73_30.xls'; 'ROI12_26_-60_55.xls';...
%     'ROI13_-31_-81_15.xls'; 'ROI14_-41_-63_-13.xls'; 'ROI15_24_-68_47.xls'; 'ROI16_31_-49_49.xls';...
%     'ROI17_-28_-70_-14.xls'; 'ROI18_-25_-54_47.xls'; 'ROI19_39_-39_48.xls'; 'ROI21_-27_-72_29.xls';...
%     'ROI22_-08_18_46.xls'; 'ROI23_28_-69_-10.xls'; 'ROI24_-30_-53_-17.xls'; 'ROI25_29_22_03.xls';...
%     'ROI26_27_-46_-16.xls'; 'ROI27_32_-57_-14.xls'; 'ROI28_-36_20_-01.xls'; 'ROI29_06_20_41.xls';...
%     'ROI30_-43_05_30.xls'; 'ROI31_43_-50_47.xls'; 'ROI32_39_05_31.xls'; 'ROI33_-06_29_40.xls';...
%     'ROI34_32_-04_50.xls'; 'ROI35_-41_-37_44.xls'; 'ROI36_57_-15_34.xls'; 'ROI37_-29_20_09.xls';...
%     'ROI38_49_-22_42.xls'; 'ROI39_-22_09_-05.xls';};

% infile = {'R01.xls'; 'R02.xls'; 'R03.xls'; 'R04.xls'; 'R05.xls'; 'R06.xls'; 'R07.xls';...
%     'R08.xls'; 'R09.xls'; 'R10.xls'; 'R11.xls'; 'R12.xls'; 'R13.xls'; 'R14.xls';...
%     'R15.xls'; 'R16.xls'; 'R17.xls'; 'R18.xls'; 'R19.xls'; 'R20.xls'; 'R21.xls';... 
%     'R22.xls'; 'R23.xls'; 'R24.xls'; 'R25.xls'; 'R26.xls'; 'R27.xls'; 'R28.xls';...
%     'R29.xls'; 'R30.xls'; 'R31.xls'; 'R32.xls'; 'R33.xls'; 'R34.xls'; 'R35.xls';...
%     'R36.xls'; 'R37.xls'; 'R38.xls';};

infile = {'R01_norm.xls'; 'R02_norm.xls'; 'R03_norm.xls'; 'R04_norm.xls'; 'R05_norm.xls'; 'R06_norm.xls'; 'R07_norm.xls';...
    'R08_norm.xls'; 'R09_norm.xls'; 'R10_norm.xls'; 'R11_norm.xls'; 'R12_norm.xls'; 'R13_norm.xls'; 'R14_norm.xls';...
    'R15_norm.xls'; 'R16_norm.xls'; 'R17_norm.xls'; 'R18_norm.xls'; 'R19_norm.xls'; 'R20_norm.xls'; 'R21_norm.xls';... 
    'R22_norm.xls'; 'R23_norm.xls'; 'R24_norm.xls'; 'R25_norm.xls'; 'R26_norm.xls'; 'R27_norm.xls'; 'R28_norm.xls';...
    'R29_norm.xls'; 'R30_norm.xls'; 'R31_norm.xls'; 'R32_norm.xls'; 'R33_norm.xls'; 'R34_norm.xls'; 'R35_norm.xls';...
    'R36_norm.xls'; 'R37_norm.xls'; 'R38_norm.xls';};



On_t(1:38,1:5)=0;
Off_t(1:38,1:5)=0;

n=15001;
RGN = 1;
    for x=1:38;
        clear i;                %Time point at peak magnitude
        clear MAG_dif;          %difference in magnitude as absolute value of peak minus Tbound
        clear ONSET;            %Onset time point
        clear THon;             %Determined alternative threshold (magnitude value) for onset
        clear PEAKtp;           %Peak time point
        clear PEAK;             %Peak magnitude
        clear PEAKtpi;          %Peak interpolated time point (tp*1000)
        clear RISE;             %Difference in magnitude from onset to peak
        clear RUN;              %Difference in time from onset to peak
        clear SLOPE;            %Average slope from determined onset to peak
        clear t;                %Percent of peak threshold for onset time (default 0.15); can increase if 0.15*peak < min magnitude of time course
        clear Tbound;           %Minimum magnitude before peak, lower bound for onset time
        clear xi;               %Interpolated 'x' (time) value
        clear base;
        clear h;
        clear OFFSET;
        i(98)=0;
        MAG_dif(98)=0;
        ONSET(98)=0;
        OFFSET(98)=0;
        THon(98)=0;
        PEAKtp(98)=0;
        PEAK(98)=0;
        PEAKtpi(98)=0;
        RISE(98)=0;
        RUN(98)=0;
        SLOPE(98)=0;
        t(98)=0;
        Tbound(98)=0;
        base(98)=0;
        h(98)=0;
        AVG(7)=0;
        yi(n:1)=0;
        xi(n:1)=0;
        
        xi = 1:.001:16;                             
%         outfile=[infile{x}, '_interp_AVG.txt'];
        
        %Load data and assign arrays
        [ndata]=xlsread (infile{x});
        
        
            S=1:98;
            xi=1:.001:16;                                           %interpolate 1000 TPs between TP1 and TP16
            tc(:,S)=ndata(:,S+1);
            
            %interpolate
            yi(:,S) = interp1(tc(:,S),xi,'linear');

            %Compute onset times
                s1=1:14;
                
            while ONSET(s1)==0;
                base(s1) = mean([tc(1,s1); tc(16,s1)]);
                xi = 1;
        
                while xi~=0 && xi < 14000 && ttest2(base(s1),(yi(xi,s1)), 0.05)==0;
                    xi=xi+1;
                end

                On_m(S)=yi(xi,S);
                ONSET(s1)=(xi/1000);
                
                if s1 < 98;
                    s1=s1+14;
                else
                end
               
            end
            
            s1=1:14;
            S=1:98;
            while OFFSET(s1)==0;
                base(s1) = mean([tc(1,s1); tc(16,s1)]);
                xi = 15001;
                b=1;
                while xi~=0 && xi > round(mean(ONSET(s1))*1000) && ttest2(base(s1),(yi(xi,s1)),0.05)==0 && b~=0;
                    if xi==round(mean(ONSET(s1))*1000)+1
                        base(s1) = mean([tc(1,s1)]);
                        xi=15001;
                        while xi~=0 && xi > round(mean(ONSET(s1))*1000) && ttest2(base(s1),(yi(xi,s1)),0.05)==0;
                            xi=xi-1;
                            b=0;
                        end
                    else
                        xi = xi-1;
                    end
                end
                Off_m(S) = yi(xi,S);
                OFFSET(s1) = (xi/1000);
                
                if s1 < 98;
                    s1=s1+14;
                else
                end
            end
        %COLLAPSE ARRAY from 98 to 7 CONDITIONS for avg time course per condition (i.e., avg subj)
%             s1=1:14;
%             AVG_3=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
%             
%             s1=s1+14;
%             AVG_4=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
% 
%             s1=s1+14;
%             AVG_5=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
% 
%             s1=s1+14;
%             AVG_6=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
%         
%             s1=s1+14;
%             AVG_7=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
% 
%             s1=s1+14;
%             AVG_8=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
% 
%             s1=s1+14;
%             AVG_S=[ mean(tc(1,s1)); mean(tc(2,s1)); mean(tc(3,s1)); mean(tc(4,s1));...
%             mean(tc(5,s1));mean(tc(6,s1));mean(tc(7,s1));mean(tc(8,s1));mean(tc(9,s1));...
%             mean(tc(10,s1)); mean(tc(11,s1)); mean(tc(12,s1)); mean(tc(13,s1));...
%             mean(tc(14,s1)); mean(tc(15,s1)); mean(tc(16,s1))];
% 
%             AVG=[AVG_3 AVG_4 AVG_5 AVG_6 AVG_7 AVG_8 AVG_S];
% 
%         %Interpolate new array
%         A=1:7;
%         xi=1:.001:16;
%         yi2(:,A) = interp1(AVG(:,A),xi,'linear');
% 
%         %PEAK Magnitude, PEAK time
%             A=1;
%             s1=1:14;
%             while s1~=0
%                 [PEAK(A), i] = max(yi2(3001:15001,A));
%                 i=i+3000;
%                 PEAKtpi(s1)=i;
%                 PEAKtp(s1)=(i/1000)+1;
%                 if s1<98;
%                     A=A+1;
%                     s1=s1+14;
%                 else
%                     s1=0;
%                 end
%             end
% 
%         %SLOPE
%         s1=1:14;
%         A=1:7;
%         while s1~=0
%             RISE(A)=mean(PEAK(A))-mean(t(s1));
%             RUN(A)=mean(PEAKtp(s1))-mean(ONSET(s1));
%             SLOPE(A)=RISE(A)./RUN(A);
%             if s1<98;
%                 s1=s1+14;
%                 A=A+1;
%             else
%                 s1=0;
%             end
%         end
% 
%         %Collapse to (1,7) array for output
%             output(:,1)=ONSET(1);
%             output(:,2)=PEAKtp(1);
%             output(:,3)=PEAK(1);
%             output(:,4)=SLOPE(1);
%             
%             output(:,5)=ONSET(15);
%             output(:,6)=PEAKtp(15);
%             output(:,7)=PEAK(2);
%             output(:,8)=SLOPE(2);
%             
%             output(:,9)=ONSET(29);
%             output(:,10)=PEAKtp(29);
%             output(:,11)=PEAK(3);
%             output(:,12)=SLOPE(3);
%             
%             output(:,13)=ONSET(43);
%             output(:,14)=PEAKtp(43);
%             output(:,15)=PEAK(4);
%             output(:,16)=SLOPE(4);
%             
%             output(:,17)=ONSET(57);
%             output(:,18)=PEAKtp(57);
%             output(:,19)=PEAK(5);
%             output(:,20)=SLOPE(5);
%             
%             output(:,21)=ONSET(71);
%             output(:,22)=PEAKtp(71);
%             output(:,23)=PEAK(6);
%             output(:,24)=SLOPE(6);
%             
%             output(:,25)=ONSET(85);
%             output(:,26)=PEAKtp(85);
%             output(:,27)=PEAK(7);
%             output(:,28)=SLOPE(7);            
% 
%             save(outfile, 'output', '-ASCII')
%             output
%             outfile
        On_t(RGN,:)=[ONSET(1) ONSET(15) ONSET(29) ONSET(43) ONSET(57)]
        Off_t(RGN,:)=[OFFSET(1) OFFSET(15) OFFSET(29) OFFSET(43) OFFSET(57)]
        RGN=RGN+1
        
    end
    
output(:,:)=On_t
output2(:,:)=Off_t
save('cluster_38POS_onsets_norm.txt', 'output', '-ASCII')
save('cluster_38POS_offsets_norm.txt', 'output2', '-ASCII')